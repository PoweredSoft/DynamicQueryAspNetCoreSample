import { Injectable } from "@angular/core";
import { IQueryCriteria, IQueryExecutionResult } from "./models";
import { Observable } from "rxjs";
import { finalize } from 'rxjs/operators';
import { HttpClient } from "@angular/common/http";


@Injectable()
export class DynamicQueryService {

  constructor(private http: HttpClient) {

  }

  read<T>(route: string, criteria: IQueryCriteria): Observable<IQueryExecutionResult<T>> {
    return this.http.post<IQueryExecutionResult<T>>(route, criteria);
  }

  get<T>(route: string, page: number = null, pageSize: number = null): Observable<IQueryExecutionResult<T>> {
    return this.http.get<IQueryExecutionResult<T>>(route, {
      params: {
        page: `${page}`,
        pageSize: `${pageSize}`
      }
    });
  }
}
