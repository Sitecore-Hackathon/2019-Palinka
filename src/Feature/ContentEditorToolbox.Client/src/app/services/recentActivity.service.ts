import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable()
export class RecentActivityService {

  constructor(private http: HttpClient) { }


   getRecentActivity(){
     return this.http.get(`/sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/-/GetRecentModifications`);
   }
}