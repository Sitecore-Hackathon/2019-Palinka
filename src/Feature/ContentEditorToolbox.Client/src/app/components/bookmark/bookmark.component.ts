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
  hasError: boolean;

  constructor(public contentEditorService: ContentEditorToolService, private _clipboardService: ClipboardService) { }

  ngOnInit() {
    this.load();
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
        error: () => {
          this.hasError = true;
          setTimeout(() => { this.hasError = false }, 3000);
          this.bookmarksIsLoading = false;
        }
      }
    );
  }

  removeBookmarkedItem(itemId: string) {
    this.contentEditorService.removeBookmarkedItem(itemId).subscribe({
      next: () => { this.load() },  // success
      error: () => {
        this.hasError = true;
        setTimeout(() => { this.hasError = false }, 3000);
        this.bookmarksIsLoading = false;
      }
    });
  }

  publisItem(itemId: string) {
    this.contentEditorService.publishItem(itemId).subscribe({
      next: () => { setTimeout(() => { this.load()}, 1000); },  // success
      error: () => {
        this.hasError = true;
        setTimeout(() => { this.hasError = false }, 3000);
        this.bookmarksIsLoading = false;
      }
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
