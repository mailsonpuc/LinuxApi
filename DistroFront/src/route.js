import { BrowserRouter, Routes, Route } from 'react-router-dom';

import Header from './components/Header';

import Home from './pages/Home';
import Sobre from './pages/Sobre';
import Distros from './pages/Distros';
import AdicionarDistro from './pages/AdicionarDistro';
import AdicionarCategoria from './pages/AdicionarCategoria';
import Login from './pages/Login';
import Registrar from './pages/Registrar';
import PrivateRoute from './components/PrivateRoute'; // Componente que verifica autenticação

function RoutesApp() {
    return (
        <BrowserRouter>
            <Header />
            <Routes>
                {/* Rotas públicas */}
                <Route path="login" element={<Login />} />
                <Route path="registrar" element={<Registrar />} />

                {/* Rota privada com layout de rotas protegidas */}
                <Route element={<PrivateRoute />}>
                    {/* Todas as rotas aqui serão protegidas */}
                    <Route path="/" element={<Home />}>
                        {/* Rotas internas */}
                        <Route path="distros" element={<Distros />} />
                        <Route path="adddistro" element={<AdicionarDistro />} />
                        <Route path="addcategoria" element={<AdicionarCategoria />} />
                        <Route path="sobre" element={<Sobre />} />
                    </Route>
                </Route>

            </Routes>
        </BrowserRouter>
    );
}

export default RoutesApp;
