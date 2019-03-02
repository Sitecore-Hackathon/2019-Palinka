import { Component, OnInit } from '@angular/core';
import { GenericEntityItem } from '../../services/GenericEntityItem';
import { ContentEditorToolService } from '../../services/ContentEditorTool.service';

@Component({
  selector: 'sc-recent-activity',
  templateUrl: './recent-activity.component.html',
  styleUrls: ['./recent-activity.component.scss']
})

export class RecentActivityComponent implements OnInit {

  recentActivities: GenericEntityItem[];

  constructor(public contentEditorToolService: ContentEditorToolService) { }

  ngOnInit() {

    this.contentEditorToolService.getRecentActivity().subscribe(
      {
        next: data => {
          this.recentActivities = data as GenericEntityItem[];
        },
        error: error => {
          this.recentActivities = error;
        }
      }
    );
  }

}
