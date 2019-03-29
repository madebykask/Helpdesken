import { ActivatedRouteSnapshot, RouteReuseStrategy, DetachedRouteHandle } from '@angular/router';

interface IRouteStorageObject {
  url: string;
  handle: DetachedRouteHandle;
}

export class CaseRouteReuseStrategy implements RouteReuseStrategy {
  
  storedRouteHandles = new Map<string, DetachedRouteHandle>();

  //supported pages for saving state 
  caseTemplatePath = 'case/template/:templateId';
  casePagePath = 'case/:id';
  filePageRoutes = ['case/:caseKey/file', 'case/:caseId/file/:fileId', 'case/:caseId/logfile/:fileId'];

  allowStoreInCache = {
    [this.casePagePath]: false,
    [this.caseTemplatePath] : false
  };

  allowRetrieveFromCache = {
    [this.casePagePath]: false,
    [this.caseTemplatePath] : false
  };

  // Method is called when navigation is changed. Allows to check if attach or detach is required by checking from/to urls.
  shouldReuseRoute(prev: ActivatedRouteSnapshot, cur: ActivatedRouteSnapshot): boolean {
    
    //check only when its last  routes in hierarchy
    if (cur.children && cur.children.length === 0) {
      const prevPath = this.getPath(prev);
      const prevUrl = this.getUrl(prev);
      const curPath = this.getPath(cur);
      const curUrl = this.getUrl(cur);
  
      console.log(`>>> shouldReuseRoute. curUrl: ${curUrl}, curPath: ${curPath}, prevUrl: ${prevUrl}, prevPath: ${prevPath}`);
    
      // navigating from case to file
      if (prevPath in this.allowRetrieveFromCache && this.isInArray(curPath, this.filePageRoutes)) {
        this.allowStoreInCache[prevPath] = true;
        this.allowRetrieveFromCache[prevPath] = false;
      } 
      else if (curPath in this.allowRetrieveFromCache && this.isInArray(prevPath, this.filePageRoutes)) {
        this.allowStoreInCache[curPath] = false;
        this.allowRetrieveFromCache[curPath] = true;
      } else {
          //reset flags in both states storages
          Object.keys(this.allowRetrieveFromCache).forEach(key => {
            this.allowStoreInCache[key] = false;
            this.allowRetrieveFromCache[key] = false;
          });
      }
    }

    // If either of our reuseUrl and default Url are true, we want to reuse the route    
    return (prev.routeConfig === cur.routeConfig);
  }

  /** 
   * Decides when the route should be stored
   * If the route should be stored, I believe the boolean is indicating to a controller whether or not to fire this.store
   * _When_ it is called though does not particularly matter, just know that this determines whether or not we store the route
   * An idea of what to do here: check the route.routeConfig.path to see if it is a path you would like to store
   * @param route This is, at least as I understand it, the route that the user is currently on, and we would like to know if we want to store it
   * @returns boolean indicating that we want to (true) or do not want to (false) store that route
  */
  shouldDetach(route: ActivatedRouteSnapshot): boolean {
    if (route.children && route.children.length === 0) {
      const url = this.getUrl(route);
      const routePath = this.getPath(route);
      if (routePath in this.allowStoreInCache && this.allowStoreInCache[routePath]) {
        console.log(">>>shoudDetach: detaching page. Url: %s, Path: %s", url, routePath);
        return true;
      }
    }
    return false;
  }

  /**
   * Constructs object of type `RouteStorageObject` to store, and then stores it for later attachment
   * @param route This is stored for later comparison to requested routes, see `this.shouldAttach`
   * @param handle Later to be retrieved by this.retrieve, and offered up to whatever controller is using this class
   */
  store(route: ActivatedRouteSnapshot, detachedTree: DetachedRouteHandle): void {
    if (route.children && route.children.length === 0) {
      const  url = this.getUrl(route);
      const routePath = this.getPath(route);
      if (routePath in this.allowStoreInCache && this.allowStoreInCache[routePath]) {
        console.log(">>>store: store instance. Url: %s, Path: %s", url, routePath);
        this.storedRouteHandles.set(routePath, <DetachedRouteHandle>{ url: url, handle: detachedTree });
      }
    }
  }

  /**
   * Determines whether or not there is a stored route and, if there is, whether or not it should be rendered in place of requested route
   * @param route The route the user requested
   * @returns boolean indicating whether or not to render the stored route
   */
  shouldAttach(route: ActivatedRouteSnapshot): boolean {
    if (route.children && route.children.length === 0) {
      const url = this.getUrl(route);
      const routePath = this.getPath(route);
      if (this.allowRetrieveFromCache.hasOwnProperty(routePath) && this.allowRetrieveFromCache[routePath] && 
          this.storedRouteHandles.has(routePath)) {
        console.log(">>>shouldAttach: attaching existing page. Url: %s, Path: %s", url, routePath);
        return true;
      }
    }    
    return false;
  }

  /** 
   * Finds the locally stored instance of the requested route, if it exists, and returns it
   * @param route New route the user has requested
   * @returns DetachedRouteHandle object which can be used to render the component
  */
  retrieve(route: ActivatedRouteSnapshot): DetachedRouteHandle {
    if (route.children && route.children.length === 0) {
      const url = this.getUrl(route);
      const routePath = this.getPath(route);
      if (this.allowRetrieveFromCache.hasOwnProperty(routePath) && this.allowRetrieveFromCache[routePath] && 
          this.storedRouteHandles.has(routePath)) {
        console.log(">>>retrieve: retrieving instance for url: ", url);
        const data = this.storedRouteHandles.get(routePath) as IRouteStorageObject;
        if (data && data.handle && data.url === url)
          return data.handle as DetachedRouteHandle;
      }
    }
    return null;
  }

  isInArray(value, array): boolean {
    return array.indexOf(value) > -1;
  }
 
  private getPath(route: ActivatedRouteSnapshot): string {
    if (route.routeConfig !== null && route.routeConfig.path !== null) {
      return route.pathFromRoot
          .map((el: ActivatedRouteSnapshot) => el.routeConfig ? el.routeConfig.path : '')
          .filter(str => str.length > 0)
          .join('/')
          .replace('//', '/');
    }
    return '';
  }

  private getUrl(route: ActivatedRouteSnapshot) {
    let next = route;
    while (next.firstChild) {
      next = next.firstChild;
    }
    const segments: string[] = [];
    while (next) {
      segments.push(next.url.join("/"));
      next = next.parent;
    }
    let url = segments.reverse().join("/");
    return url.replace('//', '/');
  }
}