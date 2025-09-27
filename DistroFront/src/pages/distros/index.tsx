// src/components/Distros.tsx

import { useEffect, useState } from "react";
import './distros.css';
import Loading from "../../components/loading";

import { type Distro } from '../../interfaces/Idistros';
import { type Categoria } from '../../interfaces/Icategoria'; // Precisamos desta interface!

export function Distros() {
    const [distros, setDistros] = useState<Distro[]>([]);
    // Novo estado para armazenar o mapa de categorias
    const [categoriasMap, setCategoriasMap] = useState<Map<string, Categoria>>(new Map());
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        // Funções assíncronas para buscar os dados
        async function fetchData() {
            setLoading(true);
            try {
                // 1. Busca as Distros
                const distrosResponse = await fetch("https://apireact-dcarhnh2g3dtdehf.brazilsouth-01.azurewebsites.net/api/Distros");
                const distrosJson: Distro[] = await distrosResponse.json();

                // 2. Busca as Categorias
                const categoriasResponse = await fetch("https://apireact-dcarhnh2g3dtdehf.brazilsouth-01.azurewebsites.net/api/Categorias");
                const categoriasJson: Categoria[] = await categoriasResponse.json();

                // 3. Cria um Map (ótimo para buscas rápidas por ID)
                const map = new Map<string, Categoria>();
                categoriasJson.forEach(cat => map.set(cat.categoriaId, cat));

                // Salva os dados nos estados
                setDistros(distrosJson);
                setCategoriasMap(map);

            } catch (error) {
                console.error("Erro ao carregar dados:", error);
            } finally {
                setLoading(false);
            }
        }

        fetchData();
    }, []);

    if (loading) {
        return <Loading />;
    }

    return (
        <div className="container">
            {distros.map((item) => {
                // Procura o nome da categoria no Map usando o categoriaId da distro
                const categoriaNome = categoriasMap.get(item.categoriaId)?.nome || "Categoria Desconhecida";

                return (
                    <article key={item.distroId} className="posts">
                        <strong className="titulo">{item.nome}</strong>
                        <img className="capa" src={item.imageUrl} alt={item.nome} />
                        <p className="subtitulo">{item.descricao}</p>

                        {/* Usa string categoriaNome */}
                        <p className="subtitulo">
                            <strong>Categoria: </strong>
                            {categoriaNome}
                        </p>

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

export default Distros;