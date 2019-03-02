import { Component, OnInit } from '@angular/core';
import { GenericEntityItem } from '../../services/GenericEntityItem';
import { SortHeaderState } from '@speak/ng-bcl/table';
import { ClipboardService } from 'ngx-clipboard';
import { ContentEditorToolService } from '../../services/ContentEditorTool.service';

@Component({
  selector: 'sc-bookmark-component',
  templateUrl: './bookmark.component.html',
  styleUrls: ['./bookmark.component.scss']
})
export class BookmarkComponent implements OnInit {

  bookmarkedItems: GenericEntityItem[];
  bookmarksIsLoading: boolean;
  hoveredItem: any;
  leftValue: any;
  topValue: any;
  hovered: boolean;

  constructor(public contentEditorService: ContentEditorToolService, private _clipboardService: ClipboardService) { }

  ngOnInit() {
    this.load();
  }

  showDetails(event, item) {

    this.hoveredItem = item;
    this.topValue = event.pageY - 50;
    this.leftValue = event.pageX;
    this.hovered = true;

  }

  unhover(event) {
    if (Math.abs(this.topValue - event.pageY) > 5 || Math.abs(this.leftValue - event.pageX) > 5 && this.hovered) {
      this.hovered = false;
      this.leftValue = 0;
      this.topValue = 0;
    }
  }

  load() {
    this.bookmarksIsLoading = true;
    this.topValue = 0;
    this.leftValue = 0;
    this.contentEditorService.getBookmarkedItems().subscribe(
      {
        next: data => {
          this.bookmarkedItems = data as GenericEntityItem[];
          this.bookmarksIsLoading = false;
        },
        error: error => {
          this.bookmarkedItems = error;
          this.bookmarksIsLoading = false;
        }
      }
    );
  }

  removeBookmarkedItem(itemId: string) {
    this.contentEditorService.removeBookmarkedItem(itemId).subscribe({
      next: result => { },  // success
      error: error => { }   // fail silently
    });
  }

  trackByItemName(id: string, header): any { return header.ItemName; }

  onSortChange(sortState: SortHeaderState[]) {

    this.bookmarkedItems.sort((a, b) => {
      let result = 0;
      sortState.forEach(header => {
        if (result !== 0) {
          return;
        }
        if (a[header.id] < b[header.id]) {
          if (header.direction === 'asc') {
            result = -1;
          } else if (header.direction === 'desc') {
            result = 1;
          }
        } else if (a[header.id] > b[header.id]) {
          if (header.direction === 'asc') {
            result = 1;
          } else if (header.direction === 'desc') {
            result = -1;
          }
        }
      });
      return result;
    });
  }
}
