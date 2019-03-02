import { Component, OnInit } from '@angular/core';
import { RecentActivityItem } from '../../services/recentActivity';
import { RecentActivityService } from '../../services/recentActivity.service';
import { GenericEntityItem } from '../../services/GenericEntityItem';

@Component({
  selector: 'sc-recent-activity',
  templateUrl: './recent-activity.component.html',
  styleUrls: ['./recent-activity.component.scss']
})
export class RecentActivityComponent implements OnInit {

  recentActivities: GenericEntityItem[];

  constructor(public recentActivityService: RecentActivityService) { }

  ngOnInit() {
    

    this.recentActivityService.getRecentActivity().subscribe(
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
