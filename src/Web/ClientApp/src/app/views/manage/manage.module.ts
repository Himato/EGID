import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ManageRoutingModule } from "./manage-routing.module";

import { ManageComponent } from "./manage.component";
import { ChangePinComponent } from "./change-pin/change-pin.component";
import { UpdateInfoComponent } from "./update-info/update-info.component";
import { SignDocComponent } from "./sign-doc/sign-doc.component";
import { VerifySignatureComponent } from "./verify-signature/verify-signature.component";

@NgModule({
  declarations: [
    ManageComponent,
    ChangePinComponent,
    UpdateInfoComponent,
    SignDocComponent,
    VerifySignatureComponent
  ],
  imports: [CommonModule, ManageRoutingModule]
})
export class ManageModule {}
