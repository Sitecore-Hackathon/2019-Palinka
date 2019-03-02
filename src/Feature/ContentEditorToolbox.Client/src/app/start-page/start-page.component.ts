import { Component, OnInit } from '@angular/core';
import { BookmarkService } from '../services/bookmark.service';
import { SortHeaderState } from '@speak/ng-bcl/table';
import { RecentActivityService } from '../services/recentActivity.service';
import { RecentActivityItem } from '../services/recentActivity';

@Component({
  selector: 'app-start-page',
  templateUrl: './start-page.component.html',
  styleUrls: ['./start-page.component.scss']
})
export class StartPageComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    

  }

 

}
