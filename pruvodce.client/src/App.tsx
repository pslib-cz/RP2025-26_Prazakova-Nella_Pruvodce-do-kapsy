import './App.css';
import { Routes, Route } from 'react-router-dom';
import IntroPage from './Pages/IntroPage';
import MapPage from './Pages/MapPage';

const App = () => {
  return (
      <Routes>
        <Route path="/" element={<IntroPage />} />
        <Route path="/map/:buildingId" element={<MapPage />} />

      </Routes>
  );
};

export default App;
