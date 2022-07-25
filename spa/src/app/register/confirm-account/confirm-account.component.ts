import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'confirm-account',
  templateUrl: './confirm-account.component.html',
  styleUrls: ['./confirm-account.component.css']
})
export class ConfirmAccountComponent implements OnInit {
  registrationComplete: boolean;

  constructor(private accountService: AccountService,  private route: ActivatedRoute) { }

  ngOnInit(): void {
    const activationId = this.route.snapshot.params['activationId']
    this.accountService.confirm(activationId)
    .subscribe({
      next: (response) => {
        this.registrationComplete = true;
      },
      error: (e) => {
        this.registrationComplete = false;
      } 
    });
  }


}
