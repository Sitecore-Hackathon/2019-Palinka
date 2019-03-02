import { Component, OnInit } from '@angular/core';
import { ContentEditorToolService } from '../services/ContentEditorTool.service';

@Component({
  selector: 'sc-locked-item-page',
  templateUrl: './locked-item-page.component.html',
  styleUrls: ['./locked-item-page.component.scss']
})
export class LockedItemPageComponent implements OnInit {

  constructor(public contentEditorToolService: ContentEditorToolService) { }

  ngOnInit() {
  }
}
