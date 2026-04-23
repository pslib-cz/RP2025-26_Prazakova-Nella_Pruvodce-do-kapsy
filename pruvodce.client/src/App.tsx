import './App.css';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import IntroPage from './Pages/IntroPage';
import MapPage from './Pages/MapPage';
// import QuizPage from './pages/QuizPage';

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<IntroPage />} />
        <Route path="/map/:buildingId" element={<MapPage />} />
        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;
