import { useEffect, useState } from 'react';
import './App.css';
import TestApi from './api/testApi';

function App() {

  const[data, setData] = useState("");

useEffect(()=>{
  TestApi({ setData })
  },[])


  return (
    <div className="App">
      <h1>{data.Email}</h1>
    </div>
  );
}

export default App;
