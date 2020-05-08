import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { AppRoutingModule } from "./app-routing.module";

import { AppComponent } from "./app.component";

import { MainModule } from "./views/main/main.module";
import { AdminModule } from "./views/admin/admin.module";
import { CivilAffairsModule } from "./views/civil-affairs/civil-affairs.module";
import { RecordsModule } from "./views/health/records/records.module";
import { AddModule } from "./views/health/add/add.module";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { ManageModule } from "./views/manage/manage.module";
import { AppHttpInterceptor } from "./core/interceptor";

@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    MainModule,
    AdminModule,
    CivilAffairsModule,
    RecordsModule,
    AddModule,
    ManageModule,
  ],
  declarations: [AppComponent],
  bootstrap: [AppComponent],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AppHttpInterceptor, multi: true },
  ],
})
export class AppModule {}
