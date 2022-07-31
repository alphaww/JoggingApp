import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private toastr: ToastrService) {}

  canActivate() {
        var user = this.accountService.currentUser;
        if (user) return true;
        this.toastr.error('401 Unauthorized') 
        return false;
  }
  
}
