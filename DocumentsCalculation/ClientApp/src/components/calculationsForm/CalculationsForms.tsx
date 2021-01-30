import React, { ChangeEvent, useCallback, useState } from 'react';

import { Button, TextField } from '@material-ui/core';

import { FormModel } from '../../models/FormModel';
import { nameof } from '../../utility/nameof';
import { useCalculationsFormStyles } from './useCalculationsFormStyles';

export const CalculationsForm: React.FC = (): JSX.Element => {
  const classes: Record<"root" | "input", string> = useCalculationsFormStyles();
  const [formModel, setFormModel] = useState<FormModel>({
    currencies: "",
    customer: "",
    outputCurrency: "",
    uploadedFile: undefined,
  });

  const handleInputChange = useCallback(
    (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>): void => {
      setFormModel({ ...formModel, [event.target.name]: event.target.value });
    },
    [formModel, setFormModel]
  );

  const handleFileUpload = useCallback(
    (event: ChangeEvent<HTMLInputElement>): void => {
      const file: File | undefined = event.target.files
        ? event.target.files[0]
        : undefined;
      setFormModel({ ...formModel, uploadedFile: file });
    },
    [formModel, setFormModel]
  );

  const handleSubmit = useCallback(
    (event: React.FormEvent<HTMLFormElement>): void => {
      event.preventDefault();

      const data: FormData = new FormData();
      data.append(nameof<FormModel>("currencies"), formModel.currencies);
      data.append(nameof<FormModel>("customer"), formModel.customer);
      data.append(
        nameof<FormModel>("outputCurrency"),
        formModel.outputCurrency
      );
      if (formModel.uploadedFile) {
        data.append(nameof<FormModel>("uploadedFile"), formModel.uploadedFile);
      }

      fetch("/documentsCalculation/calculate", { method: "POST", body: data });
    },
    [formModel]
  );

  return (
    <form
      encType="multipart/form-data"
      className={classes.root}
      autoComplete="off"
      onSubmit={handleSubmit}
    >
      <TextField
        required
        label="Currencies and exchange rates"
        name={nameof<FormModel>("currencies")}
        value={formModel.currencies}
        onChange={handleInputChange}
      />
      <TextField
        required
        label="Output currency"
        name={nameof<FormModel>("outputCurrency")}
        value={formModel.outputCurrency}
        onChange={handleInputChange}
      />
      <TextField
        label="Customer"
        name={nameof<FormModel>("customer")}
        value={formModel.customer}
        onChange={handleInputChange}
      />
      <div className={classes.root}>
        <input
          accept=".csv"
          className={classes.input}
          id="contained-button-file"
          name={nameof<FormModel>("uploadedFile")}
          type="file"
          onChange={handleFileUpload}
        />
        <label htmlFor="contained-button-file">
          <Button variant="contained" color="primary" component="span">
            Upload
          </Button>
        </label>
        <span>{formModel.uploadedFile?.name}</span>
      </div>

      <Button variant="contained" color="primary" type="submit">
        Calculate
      </Button>
    </form>
  );
};