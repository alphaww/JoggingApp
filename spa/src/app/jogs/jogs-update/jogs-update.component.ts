import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { JogService } from 'src/app/services/jog.service';

@Component({
  selector: 'app-jogs-update',
  templateUrl: './jogs-update.component.html',
  styleUrls: ['./jogs-update.component.css']
})
export class JogsUpdateComponent implements OnInit {
  jogUpdateForm: FormGroup;

   public get jogDate() {
    return this.jogUpdateForm.controls['date'].value
   }

  constructor(private jogService: JogService, private toastr: ToastrService, private fb: FormBuilder, private route: ActivatedRoute, private router: Router) {
    this.jogUpdateForm = this.fb.group({
      id: [''],
      date: [''],
      distance: ['', Validators.required],
      time: ['', Validators.required]
    })
  }

  ngOnInit(): void {
    const jogId = this.route.snapshot.params['jogId']
    this.jogService.get(jogId).subscribe(response => {
      this.jogUpdateForm.patchValue(response);
    }) 
  }

  update() {
    this.jogService.update(this.jogUpdateForm.value)
    .subscribe({
      next: (response) => {
        this.toastr.success('Successfully updated existing jog record.', 'Jog entry updated')
        this.router.navigateByUrl('jogs');
      },
      error: (e) => console.error(e)
    });
  }

  cancel() {
    this.router.navigateByUrl('jogs');
  }
}
