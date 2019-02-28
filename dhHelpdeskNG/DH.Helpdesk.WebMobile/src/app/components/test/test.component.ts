import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MbscListviewOptions, MbscListview } from '@mobiscroll/angular';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {
 
  listviewSettings: MbscListviewOptions = {
    theme: 'ios',
    swipe: false,
    enhance: true
  };

  constructor(private router: Router) {
  }

  ngOnInit() {
  }

  onLogout() {
    this.router.navigateByUrl('/login');
  }

  data = [{
    cl: 'md-continent micons icon-north-america',
    name: 'North America',
    children: [{
      imgsrc: 'https://img.mobiscroll.com/demos/flags/US.png',
      name: 'USA',
      children: [{
        name: 'Washington'
      }, {
        name: 'Florida'
      }, {
        name: 'Los Angeles'
      }, {
        name: 'New York'
      }, {
        name: 'Detroit'
      }, {
        name: 'Chicago'
      }]
    }, {
      imgsrc: 'https://img.mobiscroll.com/demos/flags/CA.png',
      name: 'Canada',
      children: [{
        name: 'Vancouver'
      }, {
        name: 'Winnipeg'
      }, {
        name: 'Calgary'
      }, {
        name: 'Montreal'
      }, {
        name: 'Quebec'
      }]
    }]
  }, {
    cl: 'md-continent micons icon-south-america',
    name: 'South America',
    children: [{
      imgsrc: 'https://img.mobiscroll.com/demos/flags/ar.png',
      name: 'Argentina',
      children: [{
        name: 'Buenos Aire'
      }, {
        name: 'Córdoba'
      }, {
        name: 'Rosario'
      }, {
        name: 'Mendoza'
      }]
    }, {
      imgsrc: 'https://img.mobiscroll.com/demos/flags/br.png',
      name: 'Brazil',
      children: [{
        name: 'Rio de Janeiro'
      }, {
        name: 'Sao Paolo'
      }, {
        name: 'Brasília'
      }, {
        name: 'Salvador'
      }, {
        name: 'Fortaleza'
      }]
    }, {
      imgsrc: 'https://img.mobiscroll.com/demos/flags/cl.png',
      name: 'Chile',
      children: [{
        name: 'Santiago'
      }, {
        name: 'Concepción'
      }, {
        name: 'Valparaíso'
      }]
    }]
  } // Showing partial data. Download full source.
  ];

}
