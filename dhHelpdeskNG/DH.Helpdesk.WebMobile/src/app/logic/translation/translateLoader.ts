import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TranslateLoader } from '@ngx-translate/core';
import { TranslationApiService } from 'src/app/services/api/translation/translation-api.service';


@Injectable()
export class CustomTranslateLoader implements TranslateLoader  {

    constructor(private translationApiService: TranslationApiService) {
        // this.logger.log('CustomTranslateLoader created.')
    }

    getTranslation(lang: string): Observable<Object> {
        // console.log('>>>Loading translations for: ' + lang);
        return this.translationApiService.getTranslations(lang);
    }
}

export function HttpLoaderFactory(translationApiService: TranslationApiService) {
    return new CustomTranslateLoader(translationApiService);
    // return new TranslateHttpLoader(http);
}
