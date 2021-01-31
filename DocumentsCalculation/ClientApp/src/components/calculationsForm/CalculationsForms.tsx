import React, { ChangeEvent, useCallback, useState } from 'react';

import { Button, TextField } from '@material-ui/core';

import { CalculatedInvoiceModel } from '../../models/CalculatedInvoiceModel';
import { CalculationResultModel } from '../../models/CalculationResultModel';
import { DataState } from '../../models/DataState';
import { FormModel } from '../../models/FormModel';
import { CalculationService } from '../../services/CalculationService';
import { nameof } from '../../utility/nameof';
import { CalculationResult } from '../calculationResult/CalculationResult';
import { useCalculationsFormStyles } from './useCalculationsFormStyles';

export const CalculationsForm: React.FC = (): JSX.Element => {
  const classes: Record<"root" | "input", string> = useCalculationsFormStyles();
  const [formModel, setFormModel] = useState<FormModel>({
    currencies: "",
    customer: "",
    outputCurrency: "",
    uploadedFile: undefined,
  });
  const [
    calculationResult,
    setCalculationResult,
  ] = useState<CalculationResultModel>({
    state: DataState.idle,
    calculatedInvoice: [],
    error: "",
  });

  const handleInputChange = useCallback(
    (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>): void => {
      setCalculationResult({
        ...calculationResult,
        calculatedInvoice: [],
        state: DataState.idle,
      });
      setFormModel({ ...formModel, [event.target.name]: event.target.value });
    },
    [formModel, calculationResult, setFormModel, setCalculationResult]
  );

  const handleFileUpload = useCallback(
    (event: ChangeEvent<HTMLInputElement>): void => {
      const file: File | undefined = event.target.files
        ? event.target.files[0]
        : undefined;

      setCalculationResult({
        ...calculationResult,
        calculatedInvoice: [],
        state: DataState.idle,
      });
      setFormModel({ ...formModel, uploadedFile: file });
    },
    [formModel, calculationResult, setFormModel, setCalculationResult]
  );

  const handleSubmit = useCallback(
    async (event: React.FormEvent<HTMLFormElement>): Promise<void> => {
      event.preventDefault();
      setCalculationResult((prevResult) => ({
        ...prevResult,
        state: DataState.pending,
      }));

      try {
        const calculationResult: CalculatedInvoiceModel[] = await CalculationService.calculateDocuments(
          formModel
        );
        setCalculationResult((prevResult) => ({
          ...prevResult,
          state: DataState.completed,
          calculatedInvoice: calculationResult,
        }));
      } catch (err) {
        setCalculationResult((prevResult) => ({
          ...prevResult,
          state: DataState.error,
          error: err.message || err.title,
        }));
      }
    },
    [formModel, setCalculationResult]
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
          <Button variant="contained" component="span">
            Upload
          </Button>
        </label>
        <span>{formModel.uploadedFile?.name}</span>
      </div>

      <Button variant="contained" color="primary" type="submit">
        Calculate
      </Button>

      <CalculationResult
        calculationResult={calculationResult}
        outputCurrency={formModel.outputCurrency}
      />
    </form>
  );
};
