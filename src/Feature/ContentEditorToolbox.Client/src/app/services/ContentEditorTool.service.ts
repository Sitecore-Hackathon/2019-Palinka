import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable()
export class ContentEditorToolService {

  constructor(private http: HttpClient) { }

   getBookmarkedItems(){
     return this.http.get(`/sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/-/GetBookmarks`);
   }

   removeBookmarkedItem(itemId) {    
    return this.http.request('delete',`/sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/${itemId}/`, {body:{Id:itemId}});
  }

  getRecentActivity(){
    return this.http.get(`/sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/-/GetRecentModifications`);
  }
}
