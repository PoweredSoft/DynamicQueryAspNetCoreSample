import { Component, OnChanges, SimpleChanges, DoCheck, KeyValueDiffers, KeyValueDiffer } from "@angular/core";
import { DynamicQueryService } from "src/dynamic-query/dynamic-query.service";
import { IQueryCriteria, ISort, IGroup, IAggregate, IFilter, ISimpleFilter } from "src/dynamic-query/models";
import { Subject } from "rxjs";

@Component({
    selector: 'page-home',
    templateUrl: './home.component.html'
})
export class HomeComponent
{
   
    criteria: IQueryCriteria = {
        filters: [],
        sorts: [],
        groups: [],
        aggregates: [],
        pageSize: 2,
        page: 1
    };

    route: string = 'api/orders';
    routes: string[] = ['api/orders', 'api/order-items', 'api/items', 'api/customers'];
    lastResult: any;

    filterTypes: string[] = [
        'Equal',
        'Contains',
        'StartsWith',
        'EndsWith',
        // not support in sample 'Composite',
        'NotEqual',
        'GreaterThan',
        'LessThanOrEqual',
        'GreaterThanOrEqual',
        'LessThan',
        'In',
        'NotIn'
    ];

    aggregateTypes: string[] = [
        'Count',
        'Sum',
        'Avg',
        'LongCount',
        'Min',
        'Max',
        'First',
        'FirstOrDefault',
        'Last',
        'LastOrDefault'
    ];

    constructor(private dynamicQueryService: DynamicQueryService) {
  
    }

    removeSort(sort: ISort) {
        this.criteria.sorts = this.criteria.sorts.filter(t => t != sort);
    }

    removeGroup(group: IGroup) {
        this.criteria.groups = this.criteria.groups.filter(t => t != group);
    }

    removeAggregate(aggregate: IAggregate) {
        this.criteria.aggregates = this.criteria.aggregates.filter(t => t != aggregate);
    }

    removeFilter(filter: IFilter) {
        this.criteria.filters = this.criteria.filters.filter(t => t != filter);
    }

    go() {
        this.lastResult = "loading...";

        this.criteria.filters
            .filter(t => t.type == 'In' || t.type == 'NotIn')
            .forEach(t => {
                let simpleFilter = <ISimpleFilter>t;
                simpleFilter.value = (simpleFilter.value as string).split(',');
            });

        this.dynamicQueryService.execute<any>(this.route, this.criteria)
            .subscribe(
                res => this.lastResult = res,
                err => this.lastResult = err
            );
    }
}