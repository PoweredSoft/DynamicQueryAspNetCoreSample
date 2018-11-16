import { Component, OnChanges, SimpleChanges, DoCheck, KeyValueDiffers, KeyValueDiffer, PACKAGE_ROOT_URL } from "@angular/core";
import { DynamicQueryService } from "src/dynamic-query/dynamic-query.service";
import { IQueryCriteria, ISort, IGroup, IAggregate, IFilter, ISimpleFilter } from "src/dynamic-query/models";
import { Subject } from "rxjs";

@Component({
  selector: 'page-home',
  templateUrl: './home.component.html'
})
export class HomeComponent {
  defaultCriteria: IQueryCriteria = {
    filters: [],
    sorts: [],
    groups: [],
    aggregates: [],
    pageSize: 20,
    page: 1
  };

  criteria: IQueryCriteria;

  route: string = 'api/ticket';
  routes: string[] = ['api/order', 'api/orderitem', 'api/item', 'api/customer', 'api/ticket', 'api/task'];
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
    this.reset();
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

  read() {
    this.lastResult = "loading...";

    this.criteria.filters
      .filter(t => t.type == 'In' || t.type == 'NotIn')
      .forEach(t => {
        let simpleFilter = <ISimpleFilter>t;
        simpleFilter.value = (simpleFilter.value as string).split(',');
      });

    let finalRoute = `${this.route}/read`;
    this.dynamicQueryService.read<any>(finalRoute, this.criteria)
      .subscribe(
        res => this.lastResult = res,
        err => this.lastResult = err
      );
  }

  get() {
    this.lastResult = "loading...";
    this.dynamicQueryService.get<any>(this.route, this.criteria.page, this.criteria.pageSize)
      .subscribe(
        res => this.lastResult = res,
        err => this.lastResult = err
      );
  }


  filteringSample() {
    if (this.route == 'api/ticket') {
      this.criteria = {
        page: 1,
        pageSize: 20,
        aggregates: [],
        filters: [
          <ISimpleFilter>{
            type: 'Equal',
            value: 'critical',
            path: 'priority'
          },
          <ISimpleFilter>{
            type: 'Equal',
            value: 'refused',
            path: 'ticketType',
            and: true
          },
        ],
        groups: [],
        sorts: []
      }
    }

    this.read();
  }

  reset() {
    this.lastResult = "press get or read to start..";
    this.criteria = { ... this.defaultCriteria };
  }

  routeChanged() {
    this.reset();
  }

  groupingSample() {
    if (this.route == 'api/ticket') {
      this.criteria = {
        aggregates: [
          { type: "Count" },
          { type: "Avg", path: "actualDuration" },
          { type: "Avg", path: "estimatedDuration" }
        ],
        filters: [],
        groups: [{
          path: 'ticketType'
        }],
        sorts: []
      }
    }

    this.read();
  }

  aggregateSample() {

    if (this.route == 'api/ticket') {
      this.criteria = {
        page: 1,
        pageSize: 10,
        aggregates: [
          { type: "Count" },
          { type: "Avg", path: "actualDuration" },
          { type: "Avg", path: "estimatedDuration" }
        ],
        filters: [],
        groups: [],
        sorts: []
      }
    }

    this.read();
  }
}
