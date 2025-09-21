import { BrowserRouter, Routes, Route } from 'react-router-dom';

import Header from './components/Header';

import Home from './pages/Home';
import Sobre from './pages/Sobre';
import Distros from './pages/Distros';
import AdicionarDistro from './pages/AdicionarDistro';
import AdicionarCategoria from './pages/AdicionarCategoria';
import Login from './pages/Login';
import Registrar from './pages/Registrar';

//import Loading from './components/Loading';


function RoutesApp() {
    return (
        <BrowserRouter>
            <Header />
            <Routes>
                <Route path='/' element={<Home />} />
                <Route path='distros' element={<Distros />} />
                <Route path='adddistro' element={<AdicionarDistro />} />
                <Route path='addcategoria' element={<AdicionarCategoria />} />
                <Route path='sobre' element={<Sobre />} />
                <Route path='login' element={<Login />} />
                <Route path='registrar' element={<Registrar />} />
                {/* 
                <Route path='loading' element={<Loading />} /> */}

            </Routes>
        </BrowserRouter>
    )
}

export default RoutesApp;