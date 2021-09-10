import compareVersions from "compare-versions"
if (process.env.NODE_ENV === "production") {
  fetch(process.env.BASE_URL + "version.json?_d=" + encodeURI(new Date().toJSON()), {
    method: "GET",
    headers: {
      pragma: "no-cache",
      "cache-control": "no-store"
    }
  })
    .then((resp) => resp.json())
    .then((json) => {
      console.log("Server Version: " + json.version)
      console.log("Local Version: " + document.head.querySelector("[name=version]").attributes.content.value)
      if (compareVersions(json.version, document.head.querySelector("[name=version]").attributes.content.value) !== 0) {
        if (confirm("You are using outdated application, from cache. Do you want to reload the latest version from server?")) {
          // hard reloads the browser https://stackoverflow.com/questions/5721704/window-location-reload-with-clear-cache
          window.location.reload(false)
        }
      }
    })
    .catch((e) => console.error("Unable to compare version from server"))
}
