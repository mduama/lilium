import React from "react";
import "./App.css";
import Form from "./components/Form";

function App() {
  return (
    <div className="App container">
      <header className="App-header">
        <img alt="liliumLogo" src="lilium-300x300.png" />
        <h1>Crew on Demand</h1>
      </header>
      <Form />
    </div>
  );
}

export default App;
