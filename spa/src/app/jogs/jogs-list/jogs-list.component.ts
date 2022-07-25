import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
import { Jog } from 'src/app/models/jog';
import { ConfirmService } from 'src/app/services/confirm.service';
import { JogService } from 'src/app/services/jog.service';

@Component({
  selector: 'app-jogs-list',
  templateUrl: './jogs-list.component.html',
  styleUrls: ['./jogs-list.component.css']
})
export class JogsListComponent implements OnInit {
  jogs: Jog[] = []
  searchByDateRange: Date[]

  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private jogService: JogService, private confirmService: ConfirmService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.search()
    this.bsConfig = {
      containerClass: 'theme-red',
      dateInputFormat: 'DD.MM.YYYY',
      rangeInputFormat: 'DD.MM.YYYY'
    }
  }

  search() {
    let dateTo = null;
    let dateFrom = null;
    if (this.searchByDateRange)
    {
      dateTo = this.searchByDateRange[1];
      dateFrom = this.searchByDateRange[0];
      
    }
    this.jogService.search(dateFrom, dateTo).subscribe(response => {
      this.jogs = response;
    });
  }

  delete(jog: Jog) {
    this.confirmService.confirm('Confirm delete jog', 'This cannot be undone').subscribe(result => {
      if (result) {
        this.jogService.delete(jog.id).subscribe(result => {
          this.toastr.success("Successfully deleted a jog record.", "Jog deleted");
          this.search()
        })
      }
      })
    }
}
