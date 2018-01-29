
export class Debug {
  constructor() {
    // Grab html elements from page
    this.serverDebug = document.getElementById('serverDebug')
    this.clientDebug = document.getElementById('clientDebug')

    this.clientState = {}
    this.serverState = {}
  }

  update(serverData, clientData) {
    this.serverDebug.textContent = this.format(serverData)
    this.clientDebug.textContent = this.format(clientData)
  }

  format(data) {
    // Input: JSON object representing a game object or state
    // Output: string that can show in html
    let output = []

    for(let i = 0; i < data.length; i++) {
      let object = {}
      object.id = data[i].id
      object.x = data[i].x
      object.y = data[i].y
      object.velocity = data[i].velocity
      output.push(object)
    }

    return(JSON.stringify(output, null, 2))
  }
}
