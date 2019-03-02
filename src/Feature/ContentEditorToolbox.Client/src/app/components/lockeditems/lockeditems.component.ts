import { Component, OnInit } from '@angular/core';
import { GenericEntityItem } from '../../services/GenericEntityItem';
import { ContentEditorToolService } from '../../services/ContentEditorTool.service';
import { ScDialogService } from '@speak/ng-bcl/dialog';

@Component({
  selector: 'sc-lockeditems',
  templateUrl: './lockeditems.component.html',
  styleUrls: ['./lockeditems.component.scss']
})
export class LockeditemsComponent implements OnInit {
  lockedItems: GenericEntityItem[];
  lockedItemIsLoading: boolean;
  selectedItem: any;
  hasError: boolean;

  constructor(public contentEditorToolService: ContentEditorToolService, public dialogService: ScDialogService) { }

  ngOnInit() {
    this.load();
  }

  load() {
    this.lockedItemIsLoading = true;
    this.contentEditorToolService.getLockedItems().subscribe(
      {
        next: data => {
          this.lockedItems = data as GenericEntityItem[];
          this.lockedItemIsLoading = false;
        },
        error: () => {
          this.hasError = true;
          setTimeout(() => { this.hasError = false }, 3000);
          this.lockedItemIsLoading = false;
        }
      }
    );
  }

  openDialog(unlockDialog, item) {
    this.selectedItem = item;
    this.dialogService.open(unlockDialog);
  }

  unlock() {
    this.lockedItemIsLoading = true;
    this.dialogService.close();
    this.contentEditorToolService.unlockItem(this.selectedItem.Id).subscribe(
      {
        next: () => {
          setTimeout(() => { this.load(); }, 1000);

        },
        error: () => {
          this.hasError = true;
          setTimeout(() => { this.hasError = false }, 3000);
          this.lockedItemIsLoading = false;
        }
      }
    );
  }
}
