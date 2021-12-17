import { Injectable } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { firstValueFrom } from "rxjs";

@Injectable({
    providedIn: 'root'
  })
export class TranslationService extends TranslateService {
    // getTranslations('login', () => this.login);
    async getSingleTranslation(key: string): Promise<string> {
        const translation: string = await firstValueFrom(this.get(key));
        return translation;
    }
}