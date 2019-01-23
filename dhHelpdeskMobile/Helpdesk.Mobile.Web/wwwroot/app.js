/// <reference path="oidc-client.js" />

Oidc.Log.logger = console;
Oidc.Log.level = 4;

var config = {

    authority: "https://localhost:449", // Url of IdentityServer
    client_id: "js", // should be the same as on IdentityServer
    // Url to redirect after authentication
    // and getting user approove - according to specification of OpenId Connect
    redirect_uri: "http://localhost:8111/callback.html",
    // Response Type set of tokens, received from Authorization Endpoint
    // this one uses Implicit Flow
    // http://openid.net/specs/openid-connect-core-1_0.html#Authentication
    response_type: "id_token token",
    // Getting subject id of a user, also field of profile in id_token, and receive access_token for access to  dhhelpdeskapi (see IdentityServer config)
    scope: "openid dhhelpdeskapi",
    // log out page
    post_logout_redirect_uri: "https:/localhost:8111/index.html",
    // watch IdentityServer session, true by default
    monitorSession: true,
    // check session interval in miliseconds, default - 2000
    checkSessionInterval: 30000,
    // revoke access_token in respect to standart https://tools.ietf.org/html/rfc7009
    revokeAccessTokenOnSignout: true,
    // Max time difference with server, required for token validation, by default 300
    // https://github.com/IdentityModel/oidc-client-js/blob/1.3.0/src/JoseUtil.js#L95
    clockSkew: 300,
    // load user info for profile
    loadUserInfo: false,
    silent_redirect_uri: "https:/localhost:499/silent-renew.html"
};
var mgr = new Oidc.UserManager(config);

function login() {
    mgr.signinRedirect();    
}

function api() {
    mgr.getUser().then(function (user) {
        var url = "https://localhost:449/identity";

        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
            log(xhr.status, JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}

function logout() {
    mgr.signoutRedirect();
}

document.getElementById("login").addEventListener("click", login, false);
document.getElementById("api").addEventListener("click", api, false);
document.getElementById("logout").addEventListener("click", logout, false);

//displayUser();

function requestUrl(mgr, url) {
    mgr.getUser().then(function (user) {
        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
            log(xhr.status, 200 == xhr.status ? JSON.parse(xhr.responseText) : "An error has occured.");
        }
        // добавляем заголовок Authorization с access_token в качестве Bearer - токена. 
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}

function log() {
    document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerHTML += msg + '\r\n';
    });
}