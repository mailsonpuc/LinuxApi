// src/components/Distros.tsx

import { useEffect, useState } from "react";
import './distros.css';
import Loading from "../../components/loading";

// Importa apenas a interface Distro (que já importa Categoria internamente)
import { type Distro } from '../../interfaces/Idistros';

export function Distros() {
    // Tipagem correta do estado: array de Distros
    const [distro, setDistro] = useState<Distro[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetch("http://localhost:5177/api/Distros")
            .then((r) => r.json())
            .then((json: Distro[]) => { // Usando a tipagem para o JSON
                setDistro(json);
                setLoading(false);
            })
            .catch(error => {
                console.error("Erro ao carregar distros:", error);
                setLoading(false);
            });
    }, []);

    if (loading) {
        return <Loading />;
    }

    return (
        <div className="container">
            {distro.map((item) => {
                return (
                    <article key={item.distroId} className="posts">
                        <strong className="titulo">{item.nome}</strong>
                        <img className="capa" src={item.imageUrl} alt={item.nome} />
                        <p className="subtitulo">{item.descricao}</p>
                        
                        {/* Acesso corrigido, item.categoria é um objeto com a propriedade .nome */}
                        <p className="subtitulo"><strong>Categoria: </strong>{item.categoria.nome}</p>
                        
                        <a className="botao"
                            href={item.iso}
                            target="_blank"
                            rel="noopener noreferrer">
                            Baixar a ISO
                        </a>
                    </article>
                );
            })}
        </div>
    );
}