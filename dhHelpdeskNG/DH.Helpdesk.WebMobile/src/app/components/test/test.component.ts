import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MbscSelectOptions } from '@mobiscroll/angular';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {
 
  options: MbscSelectOptions = {    
    showInput: false,
    showOnTap: true,
    showLabel: false,
    height: 20,
    minWidth: 100,
    theme: 'mobiscroll',
    input:"#notifierInput",
    filter: true,
    display:"bottom",
    headerText: 'Search users'
  };

  options2: MbscSelectOptions = {   
    showInput: true,
    showLabel: false, 
    showOnTap: true,
    filter: true,
    display:"center",    
    headerText: 'Search users2'
  };

  options3: MbscSelectOptions = {        
    showInput: true,
    showLabel: false, 
    input:'notifierInput2',
    showOnTap: true,
    filter: true,
    display:"center",
    headerText: 'Search users2'
  };

  constructor(private router: Router) {
  }

  ngOnInit() {
  }

  onLogout() {
    this.router.navigateByUrl('/login');
  }
  
  showNotifierSearch(selectCtrl) {
    selectCtrl.instance.setVal('test', true, false, true);
    selectCtrl.instance.show();
  }

  selected: any;

  items = [{
    value: 1,
    text: 'Option 1'
  }, {
      value: 2,
      text: 'Option 2'
  }, {
      value: 3,
      text: 'Option 4'
  }];

 
}
