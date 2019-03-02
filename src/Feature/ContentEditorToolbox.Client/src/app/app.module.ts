import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { ScAccountInformationModule } from '@speak/ng-bcl/account-information';
import { ScActionBarModule } from '@speak/ng-bcl/action-bar';
import { ScApplicationHeaderModule } from '@speak/ng-bcl/application-header';
import { ScButtonModule } from '@speak/ng-bcl/button';
import { ScGlobalHeaderModule } from '@speak/ng-bcl/global-header';
import { ScGlobalLogoModule } from '@speak/ng-bcl/global-logo';
import { ScIconModule } from '@speak/ng-bcl/icon';
import { ScPageModule } from '@speak/ng-bcl/page';
import { ScTabsModule } from '@speak/ng-bcl/tabs';
import { ScDropdownModule } from '@speak/ng-bcl/dropdown';
import { ScTableModule } from '@speak/ng-bcl/table'; 
import { ScMenuModule } from '@speak/ng-bcl/menu';
import { CONTEXT, DICTIONARY } from '@speak/ng-bcl';
import { ScActionControlModule } from '@speak/ng-bcl/action-control';
import { NgScModule } from '@speak/ng-sc';
import { SciAntiCSRFModule } from '@speak/ng-sc/anti-csrf';

import { ScDialogModule } from '@speak/ng-bcl/dialog';

import { SciLogoutService } from '@speak/ng-sc/logout';
import { AppComponent } from './app.component';
import { StartPageComponent } from './start-page/start-page.component';
import {ContentEditorToolService} from './services/ContentEditorTool.service';
import { BookmarkComponent } from './components/bookmark/bookmark.component';
import { RecentActivityComponent } from './components/recent-activity/recent-activity.component';
import { ClipboardModule } from 'ngx-clipboard';
import { ScProgressIndicatorPanelModule } from "@speak/ng-bcl/progress-indicator-panel";
import { RecentActivityPageComponent } from './recent-activity-page/recent-activity-page.component';
import { BookmarkPageComponent } from './bookmark-page/bookmark-page.component';
import { LockedItemPageComponent } from './locked-item-page/locked-item-page.component';
import { LockeditemsComponent } from './components/lockeditems/lockeditems.component';

@NgModule({
  declarations: [
    AppComponent,
    StartPageComponent,
    BookmarkComponent,
    RecentActivityComponent,
    RecentActivityPageComponent,
    BookmarkPageComponent,
    LockedItemPageComponent,
    LockeditemsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'home', component: StartPageComponent, pathMatch: 'full' },
      { path: 'bookmarks', component: BookmarkPageComponent },
      { path: 'recentactivities', component: RecentActivityPageComponent },
      { path: 'lockeditems', component: LockedItemPageComponent }
    ]),
    ScAccountInformationModule,
    ScActionBarModule,
    ScApplicationHeaderModule,
    ScButtonModule,
    ScGlobalHeaderModule,
    ScGlobalLogoModule,
    ScIconModule,
    ScPageModule,
    ScMenuModule,
    ScTabsModule,
    ScTableModule,
    ScDropdownModule,
    ScProgressIndicatorPanelModule,
    ScDialogModule,
    ScActionControlModule,
    ClipboardModule,
    SciAntiCSRFModule,
    NgScModule.forRoot({
      authItemId: '1023A91F-E7C0-410C-BE84-472204C71FD7',
      contextToken: CONTEXT,
      dictionaryToken: DICTIONARY
    })
  ],
  providers: [ SciLogoutService, ContentEditorToolService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
