import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthGuard } from "./guards/auth.guard";
import { AdminGuard } from "./guards/admin.guard";
import { CivilAffairsGuard } from "./guards/civil-affairs.guard";
import { ForbiddenComponent } from "./views/forbidden/forbidden.component";

const routes: Routes = [
  {
    path: "",
    pathMatch: "full",
    loadChildren: () =>
      import("./views/main/main.module").then((m) => m.MainModule),
  },
  {
    path: "forbidden",
    component: ForbiddenComponent,
  },
  {
    path: "admin",
    canActivate: [AuthGuard, AdminGuard],
    loadChildren: () =>
      import("./views/admin/admin.module").then((m) => m.AdminModule),
  },
  {
    canActivate: [AuthGuard, CivilAffairsGuard],
    path: "civil-affairs",
    loadChildren: () =>
      import("./views/civil-affairs/civil-affairs.module").then(
        (m) => m.CivilAffairsModule
      ),
  },
  {
    path: "manage",
    canActivate: [AuthGuard],
    loadChildren: () =>
      import("./views/manage/manage.module").then((m) => m.ManageModule),
  },
  {
    path: "health/records",
    loadChildren: () =>
      import("./views/health/records/records.module").then(
        (m) => m.RecordsModule
      ),
  },
  {
    canActivate: [AuthGuard],
    path: "health/add",
    loadChildren: () =>
      import("./views/health/add/add.module").then((m) => m.AddModule),
  },
  { path: "**", redirectTo: "" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
