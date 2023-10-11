import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SearchCurrencyRequest } from '../models/searchcurrencyrequest.model';
import { HttpClient,HttpParams  } from '@angular/common/http';
import { CurrencyViewModel } from '../models/currencyview.model';
import { Config } from '../models/constant';
import { ReportSearchModel } from '../models/reportsearchmodel';
import { ExchangeRateView } from '../models/exchangerateview';
import { countries } from '../models/countries';



@Injectable({
  providedIn: 'root'
})
export class CurrencyService {
  constructor(private http : HttpClient) { }
 public convertMoney(model:SearchCurrencyRequest):Observable<CurrencyViewModel> {
  
      return this.http.get<CurrencyViewModel>(`${Config.apiUrl}/GetExchangeRate/${model.from}/${model.to}/${model.money}/${model.formatedDate}`)

  }
  public getAllExchangeRates(model:ReportSearchModel):Observable<ExchangeRateView> {
    const fromdate = model.fromDate==undefined?'2023-09-01':model.fromDate;
    const todate = model.toDate==undefined?'2023-11-01':model.toDate;
    return this.http.get<ExchangeRateView>(`${Config.apiUrl}/GetCurrencyExchangeRates/${model.from}/${model.to}/${fromdate}/${todate}`)

}
// public getAllCurrencies():Observable<countries[]> {
  
//   return this.http.get<countries[]>(`${Config.apiUrl}/GetAvailableCurrencies`)

// }
}
