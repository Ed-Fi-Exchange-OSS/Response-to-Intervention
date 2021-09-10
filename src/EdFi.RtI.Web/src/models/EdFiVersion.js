export class EdFiVersionModel {
    name = ''
    value = ''
}

export const edFiVersionSuite2 = new EdFiVersionModel()
edFiVersionSuite2.name = 'Ed-Fi API Suite 2'
edFiVersionSuite2.value = 'v2'

export const edFiVersionSuite3 = new EdFiVersionModel()
edFiVersionSuite3.name = 'Ed-Fi API Suite 3'
edFiVersionSuite3.value = 'v3'

export const EdFiVersionsArray = [
    edFiVersionSuite2,
    edFiVersionSuite3,
]
