import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';
import { TestDto } from '../dtos/test-dto';
import { TestParam } from '../dtos/test-param';
import { TestGenericDto_TestOtherDto } from '../dtos/test-generic-dto_test-other-dto';

@Injectable()
// 示例服务
export class ExampleService {
	constructor(private appApiService: AppApiService) { }

	// 获取对象
	GetObject(x: number): Observable<any> {
		return this.appApiService.call<any>(
			"/api/ExampleService/GetObject",
			{
				x
			});
	}

	// 获取Dto
	GetDto(param: TestParam, a: TestGenericDto_TestOtherDto): Observable<TestDto> {
		return this.appApiService.call<TestDto>(
			"/api/ExampleService/GetDto",
			{
				param,
				a
			});
	}
}
