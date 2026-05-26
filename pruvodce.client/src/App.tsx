import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import IntroPage from './Pages/IntroPage';
import MapPage from './Pages/MapPage';

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<IntroPage />} />
        <Route path="/map/:buildingId" element={<MapPage />} />

      </Routes>
    </BrowserRouter>
  );
};

export default App;
