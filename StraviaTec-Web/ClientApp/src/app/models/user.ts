export class Usuario {
    user: string;
    password: string;
    nombre: string;
    apellido1: string;
    apellido2: string;
    fechaNacimiento: string;
    nacionalidad: string;
    foto: string;
    esOrganizador: boolean;
    esAmigo: string;

    constructor(
        User: string,
        Password: string,
        Nombre: string,
        Apellido1: string,
        Apellido2: string,
        Fecha_nacimiento: string,
        Nacionalidad: string,
        Foto: string
    ){
        this.user = User;
        this.password = Password;
        this.nombre = Nombre;
        this.apellido1 = Apellido1;
        this.apellido2 = Apellido2;
        this.fechaNacimiento = Fecha_nacimiento;
        this.nacionalidad = Nacionalidad;
        this.foto = Foto;
    }
}