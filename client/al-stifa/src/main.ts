import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { importProvidersFrom } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { tokenInterceptor } from '@core/interceptors/token.interceptor';

bootstrapApplication(AppComponent, {
  providers: [
    ...appConfig.providers!,
    importProvidersFrom(BrowserAnimationsModule),
    provideHttpClient(
      withInterceptors([tokenInterceptor])
    )
  ]
});
