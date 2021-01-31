import { FormModel } from '../models/FormModel';
import { nameof } from '../utility/nameof';

export class CalculationService {
  public static async calculateDocuments(formModel: FormModel) {
    const data: FormData = new FormData();
    data.append(nameof<FormModel>("currencies"), formModel.currencies);
    data.append(nameof<FormModel>("customer"), formModel.customer);
    data.append(nameof<FormModel>("outputCurrency"), formModel.outputCurrency);
    if (formModel.uploadedFile) {
      data.append(nameof<FormModel>("uploadedFile"), formModel.uploadedFile);
    }

    const response: Response = await fetch("/documentsCalculation/calculate", {
      method: "POST",
      body: data,
    });

    if (!response.ok) {
      return Promise.reject(await response.json());
    }

    return await response.json();
  }
}
