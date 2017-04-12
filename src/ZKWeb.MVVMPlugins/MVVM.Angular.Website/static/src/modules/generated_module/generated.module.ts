import { NgModule } from '@angular/core';
import { TestDto } from './dtos/test-dto';
import { TestOtherDto } from './dtos/test-other-dto';
import { TestParam } from './dtos/test-param';
import { ExampleService } from './services/example-service';
import { Translation_zh_CN } from './translations/zh-cn';
import { Translation_en_US } from './translations/en-us';

@NgModule({
	providers: [
		TestDto,
		TestOtherDto,
		TestParam,
		ExampleService,
		Translation_zh_CN,
		Translation_en_US
	]
})
export class GeneratedModule {
	public static translationModules = [
		Translation_zh_CN,
		Translation_en_US
	]
}
