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
  recentActivitiesIsLoading: boolean;
  hasError: boolean;
  constructor(public contentEditorToolService: ContentEditorToolService) { }

  ngOnInit() {
    this.load();
  }

  load() {
    this.recentActivitiesIsLoading = true;
    this.contentEditorToolService.getRecentActivity().subscribe(
      {
        next: data => {
          this.recentActivities = data as GenericEntityItem[];
          this.recentActivitiesIsLoading = false;
        },
        error: error => {
          this.hasError = true;
          setTimeout(() => { this.hasError = false }, 3000);
          this.recentActivitiesIsLoading = false;
        }
      }
    );
  }

  publishItem(itemId: string) {
    this.contentEditorToolService.publishItem(itemId).subscribe({
      next: () => { setTimeout(() => { this.load()}, 1000); },  // success
      error: () => {
        this.hasError = true;
        setTimeout(() => { this.hasError = false }, 3000);
        this.recentActivitiesIsLoading = false;
      }
    });
  }
}
