export interface IGroup
{
    path: string;
    ascending?: boolean;
}

export interface ISort
{
    path: string;
    ascending?: boolean;
}

export interface IFilter
{
    and?: boolean;
    type: string;
}

export interface ISimpleFilter extends IFilter
{
    path: string;
    value: any;
}

export interface ICompositeFilter extends IFilter
{
    filters: IFilter[];
}

export interface IAggregate
{
    path?: string;
    type: string;
}

export interface IQueryCriteria
{
    page?: number;
    pageSize?: number;
    sorts?: ISort[];
    filters?: IFilter[];
    groups?: IGroup[];
    aggregates?: IAggregate[];
}

export interface IAggregateResult
{
    path?: string;
    type: string;
    value: any;
}

export interface IQueryResult<T>
{
    data: T[] | IGroupQueryResult<T>;
    aggregates?: IAggregateResult[];
}

export interface IGroupQueryResult<T> extends IQueryResult<T>
{
    groupName: string;
    groupValue: any;
}

export interface IQueryExecutionResult<T> extends IQueryResult<T>
{
    totalRecords?: number;
    numberOfPages?: number;
}