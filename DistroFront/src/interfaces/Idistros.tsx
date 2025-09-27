// src/interfaces/Idistros.tsx

// Importa a interface da Categoria
import { type Categoria } from "./Icategoria";

export interface Distro {
    distroId: string;
    nome: string;
    descricao: string;
    imageUrl: string;
    iso: string;
    // A propriedade 'categoria' agora é do tipo Categoria (o objeto)
    categoria: Categoria;
}