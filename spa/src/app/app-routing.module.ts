import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './guards/auth.guard';
import { JogsListComponent } from './jogs/jogs-list/jogs-list.component';
import { JogsInsertComponent } from './jogs/jogs-insert/jogs-insert.component';
import { JogsUpdateComponent } from './jogs/jogs-update/jogs-update.component';
import { ConfirmAccountComponent } from './register/confirm-account/confirm-account.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'confirm-account/:activationId', component: ConfirmAccountComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'jogs', component: JogsListComponent},
      {path: 'jogs/insert', component: JogsInsertComponent},
      {path: 'jogs/update/:jogId', component: JogsUpdateComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
