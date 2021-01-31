import React, { ChangeEvent, useCallback, useState } from 'react';

import { Button, List, ListItemText, TextField } from '@material-ui/core';

import { CalculatedInvoiceModel } from '../../models/CalculatedInvoiceModel';
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
  const [calculatedInvoice, setCalculatedInvoice] = useState<
    CalculatedInvoiceModel[]
  >([]);

  const handleInputChange = useCallback(
    (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>): void => {
      setCalculatedInvoice([]);
      setFormModel({ ...formModel, [event.target.name]: event.target.value });
    },
    [formModel, setFormModel, setCalculatedInvoice]
  );

  const handleFileUpload = useCallback(
    (event: ChangeEvent<HTMLInputElement>): void => {
      const file: File | undefined = event.target.files
        ? event.target.files[0]
        : undefined;
      setCalculatedInvoice([]);
      setFormModel({ ...formModel, uploadedFile: file });
    },
    [formModel, setFormModel, setCalculatedInvoice]
  );

  const handleSubmit = useCallback(
    async (event: React.FormEvent<HTMLFormElement>): Promise<void> => {
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

      const response: Response = await fetch(
        "/documentsCalculation/calculate",
        {
          method: "POST",
          body: data,
        }
      );
      const calculationResult: CalculatedInvoiceModel[] = await response.json();
      setCalculatedInvoice(calculationResult);
    },
    [formModel, setCalculatedInvoice]
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

      <List>
        {calculatedInvoice.map((ci) => (
          <ListItemText
            key={ci.customer}
            primary={`${ci.customer} - ${formModel.outputCurrency} ${ci.total}`}
          />
        ))}
      </List>
    </form>
  );
};
