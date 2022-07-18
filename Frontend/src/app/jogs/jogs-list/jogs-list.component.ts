import { Component, OnInit } from '@angular/core';
import { Jog } from 'src/app/models/jog';
import { JogService } from 'src/app/services/jog.service';

@Component({
  selector: 'app-jogs-list',
  templateUrl: './jogs-list.component.html',
  styleUrls: ['./jogs-list.component.css']
})
export class JogsListComponent implements OnInit {
  saveMode = false;
  insertJogForm: boolean = false;
  updateJogForm: boolean = false;
 jogs: Jog[] = []

  constructor(private jogService: JogService) {
  }

  ngOnInit(): void {
    this.jogService.search(new Date(1900, 1, 1), new Date(3000, 1, 1)).subscribe(response => {
      this.jogs = response;
    });
  }




  showNewJogForm() {
    this.insertJogForm = true;
    this.saveMode = true;
  }

  cancelSaveMode(event: boolean) {
    this.saveMode = event;
  }

}
