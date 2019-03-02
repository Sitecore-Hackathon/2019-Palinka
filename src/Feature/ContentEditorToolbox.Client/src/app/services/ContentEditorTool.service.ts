import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable()
export class ContentEditorToolService {

  constructor(private http: HttpClient) { }

  getBookmarkedItems() {
    return this.http.get(`/sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/-/GetBookmarks`);
  }

  removeBookmarkedItem(itemId: string) {
    return this.http.request('delete', `/sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/${itemId}/`, { body: { Id: itemId } });
  }

  getRecentActivity() {
    return this.http.get(`/sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/-/GetRecentModifications`);
  }

  getLockedItems() {
    return this.http.get(`/sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/-/GetMyLockedItems`);
  }

  unlockItem(itemId: string) {
    return this.http.request('post', `/sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/${itemId}/UnLock`, { body: { Id: itemId } });
  }

  unlockAll() {
    return this.http.request('post',`/sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/-/UnlockAll`);
  }

  publishItem(itemId: string) {
    return this.http.request('post', `/sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/${itemId}/PublishItem`, { body: { Id: itemId } });
  }
}
