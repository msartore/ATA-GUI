function redirection() {
  counter--;
  elem.innerText = placeholder + counter + "s";

  if (counter > 0) {
    setTimeout(redirection, 1000);
  } else {
    window.location.href = "https://msartore.dev/projects/ata-gui";
  }
}

const elem = document.getElementById("r_counter");
const placeholder = "Redirection in: ";

elem.innerText = placeholder + "5s";

counter = 5;

setTimeout(redirection, 1000);
