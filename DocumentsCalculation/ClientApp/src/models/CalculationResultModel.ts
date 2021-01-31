import { CalculatedInvoiceModel } from './CalculatedInvoiceModel';
import { DataState } from './DataState';

export type CalculationResultModel = {
  state: DataState;
  calculatedInvoice: CalculatedInvoiceModel[];
  error: string;
};
